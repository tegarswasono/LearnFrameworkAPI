import { inject } from 'vue'
import ApiHelper from '../apiHelper'
import type { IPagination, IGeneralSuccessResponse } from '../apiModel'
import type { IUserModel, IUserModelCreateOrUpdate } from '../user/userModel'

export class UserApi {
  private apiHelper: ApiHelper
  private endpoint: string = 'configuration/User'

  constructor() {
    this.apiHelper = inject('$apiHelper') as ApiHelper
  }

  public async getAll(
    sortBy: string,
    descending: boolean,
    page: number,
    rowsPerPage: number
  ): Promise<IPagination<IUserModel>> {
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

  public async getById(id: string): Promise<IUserModel> {
    const output = await this.apiHelper.callApi(this.endpoint + '/' + id, 'GET')
    return output
  }

  public async create(input: IUserModelCreateOrUpdate): Promise<IGeneralSuccessResponse> {
    const output = await this.apiHelper.callApi(this.endpoint + '/Create', 'POST', input)
    return output
  }

  public async update(input: IUserModelCreateOrUpdate): Promise<IGeneralSuccessResponse> {
    const output = await this.apiHelper.callApi(this.endpoint + '/Update', 'PUT', input)
    return output
  }

  public async delete(id: string): Promise<IGeneralSuccessResponse> {
    const output: IGeneralSuccessResponse = await this.apiHelper.callApi(
      this.endpoint + `/${id}/delete`,
      'DELETE'
    )
    return output
  }

  //   public async resetPassword(model: IUsersResetPasswordInput): Promise<IGeneralSuccessResponse> {
  //     var output = await this.apiHelper.callApi(this.endpoint + '/ResetPassword', 'POST', model)
  //     return output
  //   }

  //   public async getRolesDatasource(): Promise<IGeneralDatasourceOutput[]> {
  //     var output = await this.apiHelper.callApi(this.endpoint + '/Datasources/Roles', 'GET')
  //     return output
  //   }

  //   public async getActionRolesDatasource(): Promise<IGeneralDatasourceOutput[]> {
  //     var output = await this.apiHelper.callApi(this.endpoint + '/Datasources/ActionRoles', 'GET')
  //     return output
  //   }
}
