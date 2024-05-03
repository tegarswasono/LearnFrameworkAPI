interface IBaseEntity {
  id: string
  createBy: string
  createAt: Date
  updateBy?: string
  updateAt?: Date
}
interface IBaseListEntity {
  id: string
  sortBy: string
  descending: boolean
  page: number
  rowsPerPage: number

  rowsNumber: number
  createAt: Date
  updateAt?: Date
}

interface IPagination<T> {
  total: number
  page: number
  limit: number
  result: T[]
}

interface IGeneralIdInput {
  id: string
}

interface IGeneralSuccessResponse {
  message: string
}

interface IProfileChangeFullnameInput {
  id: string
  username: string
}

interface IProfileChangePasswordInput {
  newPassword: string
  oldPassword: string
}

interface IProfileSendEmailTokenInput {
  email: string
  password: string
}

interface IProfileVerifyChangeEmailInput {
  email: string
  password: string
}

interface IProfileUsersOutput extends IBaseEntity {
  id: string
  userName: string
  fullName: string
  email: string
  role: string
  isActive: boolean
}

interface IProfileUserOutput {
  id: string
  username: string
  appRole: string
  actionRole: string
  agentId: string
  agentName: string
}

interface ICreateUsersInput {
  password: string
  email: string
  fullname: string
  appRoleId: string
  actionRoleId: string
  agentId: string | null
}

interface IUpdateUsersInput extends ICreateUsersInput {
  id: string
}

interface IUsersResetPasswordInput {
  id: string
  oldPassword: string
  newPassword: string
  confirmNewPassword: string
}

interface IUsersOutput extends IBaseEntity {
  id: string
  username: string
  email: string
  appRole: string
  agentName: string
  actionRole: string
}

interface IGetUserByIdOutput extends IBaseEntity {
  username: string
  email: string
  appRoleId: string
  appRole: string
  actionRoleId: string
  actionRole: string
  agentId: string
  agentName: string
  profilePictureAttachmentId: string | null
  attachmentUrl: string | null
}

interface IGetUserList extends IBaseListEntity {
  result: IUsersOutput[]
}

export type {
  IProfileChangeFullnameInput,
  IProfileChangePasswordInput,
  IProfileSendEmailTokenInput,
  IProfileVerifyChangeEmailInput,
  IProfileUsersOutput,
  IBaseEntity,
  ICreateUsersInput,
  IUsersResetPasswordInput,
  IUpdateUsersInput,
  IUsersOutput,
  IPagination,
  IGeneralIdInput,
  IGeneralSuccessResponse,
  IProfileUserOutput,
  IGetUserByIdOutput,
  IGetUserList
}
