import { inject } from 'vue'
import ApiHelper from '../apiHelper'
import type { IGeneralDatasourceModel } from './datasourceModel'

export class DatasourceApi {
  private apiHelper: ApiHelper
  private endpoint: string = '/Api/Common/Datasource'

  constructor() {
    this.apiHelper = inject('$apiHelper') as ApiHelper
  }

  public async category(): Promise<IGeneralDatasourceModel[]> {
    const output = await this.apiHelper.callApi(this.endpoint + '/category', 'GET')
    return output
  }
}
