import { inject } from 'vue'
import ApiHelper from '../apiHelper'
import type { IGeneralSuccessResponse } from '../apiModel'
import type { IMyProfileModel, IMyProfileModelUpdate } from './myProfileModel'

export class MyProfileApi {
  private apiHelper: ApiHelper
  private endpoint: string = 'configuration/MyProfile'

  constructor() {
    this.apiHelper = inject('$apiHelper') as ApiHelper
  }
  public async get(): Promise<IMyProfileModel> {
    const output = await this.apiHelper.callApi(this.endpoint, 'GET')
    return output
  }

  public async update(input: IMyProfileModelUpdate): Promise<IGeneralSuccessResponse> {
    const output = await this.apiHelper.callApi(this.endpoint + '/Update', 'PUT', input)
    return output
  }
}
