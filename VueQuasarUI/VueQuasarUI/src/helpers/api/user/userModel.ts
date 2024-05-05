import type { IBaseEntity } from '../apiModel'
interface IUserModel extends IBaseEntity {
  username: string
  email: string
  fullName: string
  active: boolean
  activeInString: string
}
interface IUserModelCreateOrUpdate {
  id: string
  email: string
  fullName: string
  active: boolean
}

export type { IUserModel, IUserModelCreateOrUpdate }
