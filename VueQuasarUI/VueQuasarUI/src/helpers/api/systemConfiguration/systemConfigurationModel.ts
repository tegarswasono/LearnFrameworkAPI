import type { IBaseEntity } from '../apiModel'
interface ISystemConfigurationModel extends IBaseEntity {
  exampleSetting: string
}
interface ISystemConfigurationModelCreateOrUpdate {
  id: string
  exampleSetting: string
}

export type { ISystemConfigurationModel, ISystemConfigurationModelCreateOrUpdate }
