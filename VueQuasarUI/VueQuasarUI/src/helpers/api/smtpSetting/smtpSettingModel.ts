import type { IBaseEntity } from '../apiModel'
interface ISmtpSettingModel extends IBaseEntity {
  smtpServer: string
  smtpPort: number
  smtpUser: string
  smtpPassword: string
  smtpIsUseSsl: boolean
}
interface ISmtpSettingModelCreateOrUpdate {
  id: string
  smtpServer: string
  smtpPort: number | null
  smtpUser: string
  smtpPassword: string
  smtpIsUseSsl: boolean
}

export type { ISmtpSettingModel, ISmtpSettingModelCreateOrUpdate }
