import type { IBaseEntity } from '../apiModel'
interface IMyProfileModel extends IBaseEntity {
  username: string
  email: string
  fullName: string
  phoneNumber: string
  active: boolean
  activeInString: string
  profilePicture: string
}
interface IMyProfileModelUpdate {
  id: string
  fullName: string
  phoneNumber: string
}
interface IMyProfileChangePassword {
  currentPassword: string
  newPassword: string
  confirmPassword: string
}

export type { IMyProfileModel, IMyProfileModelUpdate, IMyProfileChangePassword }
