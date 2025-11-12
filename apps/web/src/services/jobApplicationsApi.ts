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
  async list(
    params: JobApplicationsQueryParams,
  ): Promise<PaginatedJobApplicationsResponse<JobApplication>> {
    const { data } = await apiClient.get<RawPaginatedResponse<RawJobApplication>>(RESOURCE, {
      params,
    })

    return mapPaginatedJobApplicationsResponse(data)
  },

  async getById(id: number): Promise<JobApplication> {
    const { data } = await apiClient.get<RawJobApplication>(`${RESOURCE}/${id}`)
    return mapJobApplication(data)
  },

  async create(payload: CreateJobApplicationPayload): Promise<JobApplication> {
    const { data } = await apiClient.post<RawJobApplication>(RESOURCE, payload)
    return mapJobApplication(data)
  },

  async update(id: number, payload: UpdateJobApplicationPayload): Promise<void> {
    await apiClient.put(`${RESOURCE}/${id}`, payload)
  },

  async changeStatus(
    id: number,
    payload: JobApplicationStatusChangePayload,
  ): Promise<void> {
    await apiClient.put(`${RESOURCE}/${id}`, payload)
  },

  async delete(id: number): Promise<void> {
    await apiClient.delete(`${RESOURCE}/${id}`)
  },
}

export default jobApplicationsApi

