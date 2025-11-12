import apiClient from './apiClient'

import type {
  CreateJobApplicationPayload,
  JobApplication,
  JobApplicationStatusChangePayload,
  JobApplicationsQueryParams,
  PaginatedJobApplicationsResponse,
  RawJobApplication,
  RawPaginatedResponse,
  UpdateJobApplicationPayload,
} from '@web/types/jobApplications'
import { mapJobApplication, mapPaginatedJobApplicationsResponse } from '@web/types/jobApplications'

const RESOURCE = '/job-applications'

const jobApplicationsApi = {
  /**
   * Retrieves paginated job applications using the provided filter params.
   *
   * @param {JobApplicationsQueryParams} params Current filter parameters.
   * @returns {Promise<PaginatedJobApplicationsResponse<JobApplication>>} Paginated response of applications.
   */
  async list(
    params: JobApplicationsQueryParams,
  ): Promise<PaginatedJobApplicationsResponse<JobApplication>> {
    const { data } = await apiClient.get<RawPaginatedResponse<RawJobApplication>>(RESOURCE, {
      params,
    })

    return mapPaginatedJobApplicationsResponse(data)
  },

  /**
   * Fetches a single job application by identifier.
   *
   * @param {number} id Identifier of the job application.
   * @returns {Promise<JobApplication>} Normalized job application.
   */
  async getById(id: number): Promise<JobApplication> {
    const { data } = await apiClient.get<RawJobApplication>(`${RESOURCE}/${id}`)
    return mapJobApplication(data)
  },

  /**
   * Creates a new job application.
   *
   * @param {CreateJobApplicationPayload} payload Payload describing the new application.
   * @returns {Promise<JobApplication>} Newly created and normalized application.
   */
  async create(payload: CreateJobApplicationPayload): Promise<JobApplication> {
    const { data } = await apiClient.post<RawJobApplication>(RESOURCE, payload)
    return mapJobApplication(data)
  },

  /**
   * Updates an existing job application.
   *
   * @param {number} id Identifier of the application to update.
   * @param {UpdateJobApplicationPayload} payload Update payload.
   * @returns {Promise<void>} Resolves when the update succeeds.
   */
  async update(id: number, payload: UpdateJobApplicationPayload): Promise<void> {
    await apiClient.put(`${RESOURCE}/${id}`, payload)
  },

  /**
   * Updates the status of an existing job application.
   *
   * @param {number} id Identifier of the application whose status changes.
   * @param {JobApplicationStatusChangePayload} payload Status update payload.
   * @returns {Promise<void>} Resolves when the status update succeeds.
   */
  async changeStatus(
    id: number,
    payload: JobApplicationStatusChangePayload,
  ): Promise<void> {
    await apiClient.put(`${RESOURCE}/${id}`, payload)
  },

  /**
   * Deletes a job application.
   *
   * @param {number} id Identifier of the application to delete.
   * @returns {Promise<void>} Resolves when the record is deleted.
   */
  async delete(id: number): Promise<void> {
    await apiClient.delete(`${RESOURCE}/${id}`)
  },
}

export default jobApplicationsApi

