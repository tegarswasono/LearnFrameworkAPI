import { inject } from 'vue'
import ApiHelper from '../apiHelper'
import type { IGeneralSuccessResponse } from '../apiModel'
import type {
  IMyProfileModel,
  IMyProfileModelUpdate,
  IMyProfileChangePassword,
  IMyMenuModel
} from './myProfileModel'

export default class MyProfileApi {
  private apiHelper: ApiHelper
  private endpoint: string = '/Api/Configuration/MyProfile'

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

  public async changePassword(input: IMyProfileChangePassword): Promise<IGeneralSuccessResponse> {
    return this.apiHelper.callApi(this.endpoint + '/ChangePassword', 'PUT', input)
  }

  public async ChangeProfilePicture(input: FormData): Promise<IGeneralSuccessResponse> {
    return this.apiHelper.callApiFormData(this.endpoint + '/ChangeProfilePicture', 'PUT', input)
  }

  public async myMenu(): Promise<IMyMenuModel> {
    return this.apiHelper.callApi(this.endpoint + '/MyMenu', 'GET')
  }
}
