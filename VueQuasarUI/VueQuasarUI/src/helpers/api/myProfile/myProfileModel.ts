import type { IBaseEntity } from '../apiModel'
interface IMyProfileModel extends IBaseEntity {
  username: string
  email: string
  fullName: string
  phoneNumber: string
  active: boolean
  activeInString: string
}
interface IMyProfileModelUpdate {
  id: string
  fullName: string
  phoneNumber: string
}

export type { IMyProfileModel, IMyProfileModelUpdate }
