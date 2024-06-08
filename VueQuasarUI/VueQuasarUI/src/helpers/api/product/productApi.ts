import { inject } from 'vue'
import ApiHelper from '../apiHelper'
import type { IPagination, IGeneralSuccessResponse } from '../apiModel'
import type { IProductModel, IProductModelCreateOrUpdate } from './productModel'

export default class ProductApi {
  private apiHelper: ApiHelper
  private endpoint: string = '/Api/Master/Product'

  constructor() {
    this.apiHelper = inject('$apiHelper') as ApiHelper
  }

  public async getAll(
    sortBy: string,
    descending: boolean,
    page: number,
    rowsPerPage: number
  ): Promise<IPagination<IProductModel>> {
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

  public async getById(id: string): Promise<IProductModel> {
    const output = await this.apiHelper.callApi(this.endpoint + '/' + id, 'GET')
    return output
  }

  public async create(input: IProductModelCreateOrUpdate): Promise<IGeneralSuccessResponse> {
    const output = await this.apiHelper.callApi(this.endpoint + '/Create', 'POST', input)
    return output
  }

  public async update(input: IProductModelCreateOrUpdate): Promise<IGeneralSuccessResponse> {
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
