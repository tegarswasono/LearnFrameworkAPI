import { inject } from 'vue'
import ApiHelper from '../apiHelper'
import type { IGeneralSuccessResponse } from '../apiModel'
import type {
  ISystemConfigurationModel,
  ISystemConfigurationModelCreateOrUpdate
} from './systemConfigurationModel'
import type { IGeneralDatasourceModel } from '../datasource/datasourceModel'

export default class SystemConfigurationApi {
  private apiHelper: ApiHelper
  private endpoint: string = '/Api/Configuration/SystemConfiguration'

  constructor() {
    this.apiHelper = inject('$apiHelper') as ApiHelper
  }
  public async get(): Promise<ISystemConfigurationModel> {
    return await this.apiHelper.callApi(this.endpoint, 'GET')
  }

  public async createOrUpdate(
    input: ISystemConfigurationModelCreateOrUpdate
  ): Promise<IGeneralSuccessResponse> {
    const output = await this.apiHelper.callApi(this.endpoint + '/CreateOrUpdate', 'PUT', input)
    return output
  }

  public async datasourceRoles(): Promise<IGeneralDatasourceModel[]> {
    return await this.apiHelper.callApi(this.endpoint + '/Datasource/Roles', 'GET')
  }
}
