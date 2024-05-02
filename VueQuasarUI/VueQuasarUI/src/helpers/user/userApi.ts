import { inject } from 'vue'
import ApiHelper from '../apiHelper'
import type {
  ICreateUsersInput,
  IUsersOutput,
  IUpdateUsersInput,
  IGeneralSuccessResponse,
  IUsersResetPasswordInput,
  IPagination,
  IGetUserByIdOutput
} from '../apiModel'
import type { IGeneralDatasourceOutput } from '../datasource/IGeneralDatasourceOutput'

export default class UsersApi {
  private apiHelper: ApiHelper
  private endpoint: string = 'configuration/User'

  constructor() {
    this.apiHelper = inject('$apiHelper') as ApiHelper
  }

  public async getAll(): Promise<any> {
    const output = await this.apiHelper.callApi(this.endpoint, 'GET')
    return output
  }

  public async getById(id: string): Promise<IGetUserByIdOutput> {
    const output = await this.apiHelper.callApi(this.endpoint + '/' + id, 'GET')
    return output
  }

  public async create(input: ICreateUsersInput): Promise<IGeneralSuccessResponse> {
    const output = await this.apiHelper.callApi(this.endpoint + '/Create', 'POST', input)
    return output
  }

  public async update(input: IUpdateUsersInput): Promise<IGeneralSuccessResponse> {
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
