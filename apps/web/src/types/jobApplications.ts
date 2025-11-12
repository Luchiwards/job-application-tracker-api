import { z } from 'zod'

export const jobApplicationStatuses = [
  'Applied',
  'Shortlisted',
  'Interview',
  'InProcess',
  'Offer',
  'Declined',
  'PositionClosed',
] as const

export type JobApplicationStatus = (typeof jobApplicationStatuses)[number]

const statusById: Record<number, JobApplicationStatus> = {
  1: 'Applied',
  2: 'Shortlisted',
  3: 'Interview',
  4: 'InProcess',
  5: 'Offer',
  6: 'Declined',
  7: 'PositionClosed',
}

export const jobApplicationStatusSchema = z.enum(jobApplicationStatuses)

/**
 * Validates that a string can be parsed into a date.
 *
 * @param {string} value Date string provided by the form.
 * @returns {boolean} Whether the value can be parsed by `Date`.
 */
const isValidDateOnly = (value: string) => !Number.isNaN(Date.parse(value))

export const jobApplicationFormSchema = z.object({
  companyName: z.string().trim().min(1, 'Company name is required').max(200, 'Company name is too long'),
  position: z.string().trim().min(1, 'Position is required').max(200, 'Position is too long'),
  status: jobApplicationStatusSchema,
  dateApplied: z
    .string()
    .min(1, 'Date applied is required')
    .refine(isValidDateOnly, 'Select a valid date applied'),
  notes: z
    .string()
    .trim()
    .max(2000, 'Notes must be 2000 characters or less')
    .nullish()
    .transform((value) => (value === undefined || value === null || value === '' ? null : value))
    .default(null),
})

export const jobApplicationCreateSchema = jobApplicationFormSchema
export const jobApplicationUpdateSchema = jobApplicationFormSchema

export type JobApplicationFormValues = z.infer<typeof jobApplicationFormSchema>

export type JobApplicationPayload = {
  companyName: string
  position: string
  status: JobApplicationStatus
  dateApplied: string
  notes: string | null
}

export type CreateJobApplicationPayload = JobApplicationPayload
export type UpdateJobApplicationPayload = JobApplicationPayload
export type JobApplicationStatusChangePayload = JobApplicationPayload

export interface JobApplication {
  id: number
  companyName: string
  position: string
  status: JobApplicationStatus
  dateApplied: string
  notes: string | null
  lastUpdatedOn: string
}

export interface RawJobApplication {
  id: number
  companyName: string
  position: string
  status: JobApplicationStatus | number
  dateApplied: string
  notes?: string | null
  lastUpdatedOn: string
}

export interface PaginatedJobApplicationsResponse<T> {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

export interface RawPaginatedResponse<T> {
  Items?: T[]
  items?: T[]
  Page?: number
  page?: number
  PageSize?: number
  pageSize?: number
  TotalCount?: number
  totalCount?: number
  TotalPages?: number
  totalPages?: number
  HasPreviousPage?: boolean
  hasPreviousPage?: boolean
  HasNextPage?: boolean
  hasNextPage?: boolean
}

export interface JobApplicationsQueryParams {
  page?: number
  pageSize?: number
  status?: JobApplicationStatus
  searchTerm?: string
  appliedFrom?: string
  appliedTo?: string
}

export const jobApplicationsQuerySchema = z.object({
  page: z.number().int().positive().optional(),
  pageSize: z.number().int().positive().max(100).optional(),
  status: jobApplicationStatusSchema.optional(),
  searchTerm: z.string().max(200).optional(),
  appliedFrom: z.string().optional(),
  appliedTo: z.string().optional(),
})

/**
 * Normalizes status values returned by the API to a union member.
 *
 * @param {JobApplicationStatus | number} value Status returned from the API.
 * @returns {JobApplicationStatus} Status in string representation.
 */
export const normalizeStatus = (value: JobApplicationStatus | number): JobApplicationStatus => {
  if (typeof value === 'string') {
    return value as JobApplicationStatus
  }

  return statusById[value] ?? 'Applied'
}

/**
 * Maps a raw API job application into the UI friendly structure.
 *
 * @param {RawJobApplication} raw Raw application returned by the API.
 * @returns {JobApplication} Normalized job application.
 */
export const mapJobApplication = (raw: RawJobApplication): JobApplication => ({
  id: raw.id,
  companyName: raw.companyName,
  position: raw.position,
  status: normalizeStatus(raw.status),
  dateApplied: toIsoDate(raw.dateApplied),
  notes: raw.notes ?? null,
  lastUpdatedOn: raw.lastUpdatedOn,
})

/**
 * Maps a raw paginated response into a normalized structure with camelCase fields.
 *
 * @param {RawPaginatedResponse<RawJobApplication>} raw Raw paginated payload.
 * @returns {PaginatedJobApplicationsResponse<JobApplication>} Normalized paginated response.
 */
export const mapPaginatedJobApplicationsResponse = (
  raw: RawPaginatedResponse<RawJobApplication>,
): PaginatedJobApplicationsResponse<JobApplication> => {
  const items = raw.items ?? raw.Items ?? []

  return {
    items: items.map(mapJobApplication),
    page: raw.page ?? raw.Page ?? 1,
    pageSize: raw.pageSize ?? raw.PageSize ?? items.length,
    totalCount: raw.totalCount ?? raw.TotalCount ?? items.length,
    totalPages: raw.totalPages ?? raw.TotalPages ?? 1,
    hasPreviousPage: raw.hasPreviousPage ?? raw.HasPreviousPage ?? false,
    hasNextPage: raw.hasNextPage ?? raw.HasNextPage ?? false,
  }
}

export const PAGE_SIZE = Number.parseInt(import.meta.env.VITE_PAGE_SIZE ?? '10', 10)

export const getJobApplicationStatusLabel = (status: JobApplicationStatus): string =>
  status.replace(/([A-Z])/g, ' $1').trim()

/**
 * Converts a date string to ISO date format if possible.
 *
 * @param {string} value Date string from API or form.
 * @returns {string} Normalized ISO date-only string.
 */
const toIsoDate = (value: string): string => {
  const date = new Date(value)
  if (Number.isNaN(date.valueOf())) {
    return value
  }
  return date.toISOString().slice(0, 10)
}

/**
 * Transforms form values into an API payload.
 *
 * @param {JobApplicationFormValues} values Form values captured from the UI.
 * @returns {JobApplicationPayload} Payload ready for transmission to the API.
 */
export const mapFormValuesToPayload = (values: JobApplicationFormValues): JobApplicationPayload => ({
  companyName: values.companyName,
  position: values.position,
  status: values.status,
  dateApplied: toIsoDate(values.dateApplied),
  notes: values.notes ?? null,
})

/**
 * Transforms a job application entity into form values for editing.
 *
 * @param {JobApplication} application Application entity from state.
 * @returns {JobApplicationFormValues} Values to seed the form.
 */
export const mapApplicationToFormValues = (application: JobApplication): JobApplicationFormValues => ({
  companyName: application.companyName,
  position: application.position,
  status: application.status,
  dateApplied: toIsoDate(application.dateApplied),
  notes: application.notes ?? null,
})

