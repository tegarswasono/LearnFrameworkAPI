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
interface IMyMenuModel {
  section: string
  child: IMyMenuModelItem
}
interface IMyMenuModelItem {
  title: string
  icon: string
  url: string
  child: IMyMenuModelItem
}

export type {
  IMyProfileModel,
  IMyProfileModelUpdate,
  IMyProfileChangePassword,
  IMyMenuModel,
  IMyMenuModelItem
}
