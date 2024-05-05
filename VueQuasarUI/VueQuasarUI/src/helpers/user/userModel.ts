import type { IBaseEntity } from '../apiModel'
interface IUserModel extends IBaseEntity {
  username: string
  email: string
  fullName: string
  isActive: boolean
}
interface IUserModelCreateOrUpdate {
  id: string
  email: string
  fullName: string
  isActive: boolean
}

export type { IUserModel, IUserModelCreateOrUpdate }
