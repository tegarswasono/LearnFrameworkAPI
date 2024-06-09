import type { IBaseEntity } from '../apiModel'
interface ISystemConfigurationModel extends IBaseEntity {
  appBaseUrl: string
  defaultRoleId: string
  exampleSetting: string
}
interface ISystemConfigurationModelCreateOrUpdate {
  id: string
  appBaseUrl: string
  defaultRoleId: string
  exampleSetting: string
}

export type { ISystemConfigurationModel, ISystemConfigurationModelCreateOrUpdate }
