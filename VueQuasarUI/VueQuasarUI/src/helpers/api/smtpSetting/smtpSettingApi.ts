import { inject } from 'vue'
import ApiHelper from '../apiHelper'
import type { IGeneralSuccessResponse } from '../apiModel'
import type { ISmtpSettingModel, ISmtpSettingModelCreateOrUpdate } from './smtpSettingModel'

export class SmtpSettingApi {
  private apiHelper: ApiHelper
  private endpoint: string = '/Api/Configuration/SmtpSetting'

  constructor() {
    this.apiHelper = inject('$apiHelper') as ApiHelper
  }
  public async get(): Promise<ISmtpSettingModel> {
    const output = await this.apiHelper.callApi(this.endpoint, 'GET')
    return output
  }

  public async createOrUpdate(
    input: ISmtpSettingModelCreateOrUpdate
  ): Promise<IGeneralSuccessResponse> {
    const output = await this.apiHelper.callApi(this.endpoint + '/CreateOrUpdate', 'PUT', input)
    return output
  }
}
