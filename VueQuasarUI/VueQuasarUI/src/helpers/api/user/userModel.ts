import type { IBaseEntity } from '../apiModel'
import type { IRoleModel } from '../role/roleModel'
interface IUserModel extends IBaseEntity {
  username: string
  email: string
  fullName: string
  phoneNumber: string
  active: boolean
  activeInString: string
  roles: IRoleModel[] | null
}
interface IUserModelCreateOrUpdate {
  id: string
  email: string
  fullName: string
  phoneNumber: string
  active: boolean
  password: string | null
  roles: IRoleModel[] | null
}

export type { IUserModel, IUserModelCreateOrUpdate }
