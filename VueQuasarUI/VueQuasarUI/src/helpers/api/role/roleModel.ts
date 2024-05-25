import type { IBaseEntity } from '../apiModel'
interface IRoleModel extends IBaseEntity {
  name: string
}
interface IRoleModelCreateOrUpdate {
  id: string
  name: string
  RoleFunctions: IRoleFunctionModel[]
}
interface IRoleFunctionModel extends IBaseEntity {
  moduleId: string
  isChecked: boolean
  items: IRoleFunctionModelItem[]
}
interface IRoleFunctionModelItem extends IBaseEntity {
  id: string
  name: string
  isChecked: boolean
}

export type { IRoleModel, IRoleModelCreateOrUpdate, IRoleFunctionModel, IRoleFunctionModelItem }
