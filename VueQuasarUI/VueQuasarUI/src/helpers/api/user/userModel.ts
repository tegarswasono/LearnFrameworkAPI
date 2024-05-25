import type { IBaseEntity } from '../apiModel'
interface IUserModel extends IBaseEntity {
  username: string
  email: string
  fullName: string
  phoneNumber: string
  active: boolean
  activeInString: string
}
interface IUserModelCreateOrUpdate {
  id: string
  email: string
  fullName: string
  active: boolean
  password: string | null
}

export type { IUserModel, IUserModelCreateOrUpdate }
