import { inject } from 'vue'
import ApiHelper from '../apiHelper'
import type { IPagination, IGeneralSuccessResponse } from '../apiModel'
import type { ICategoryModel, ICategoryModelCreateOrUpdate } from './categoryModel'

export class CategoryApi {
  private apiHelper: ApiHelper
  private endpoint: string = 'Master/Category'

  constructor() {
    this.apiHelper = inject('$apiHelper') as ApiHelper
  }

  public async getAll(
    sortBy: string,
    descending: boolean,
    page: number,
    rowsPerPage: number
  ): Promise<IPagination<ICategoryModel>> {
    const url =
      this.endpoint +
      '?sortBy=' +
      sortBy +
      '&descending=' +
      descending +
      '&page=' +
      page +
      '&rowsPerPage=' +
      rowsPerPage
    const output = await this.apiHelper.callApi(url, 'GET')
    return output
  }

  public async getById(id: string): Promise<ICategoryModel> {
    const output = await this.apiHelper.callApi(this.endpoint + '/' + id, 'GET')
    return output
  }

  public async create(input: ICategoryModelCreateOrUpdate): Promise<IGeneralSuccessResponse> {
    const output = await this.apiHelper.callApi(this.endpoint + '/Create', 'POST', input)
    return output
  }

  public async update(input: ICategoryModelCreateOrUpdate): Promise<IGeneralSuccessResponse> {
    const output = await this.apiHelper.callApi(this.endpoint + '/Update', 'PUT', input)
    return output
  }

  public async delete(id: string): Promise<IGeneralSuccessResponse> {
    const output: IGeneralSuccessResponse = await this.apiHelper.callApi(
      this.endpoint + `/${id}`,
      'DELETE'
    )
    return output
  }
}
