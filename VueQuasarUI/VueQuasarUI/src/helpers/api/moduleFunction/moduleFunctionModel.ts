import type { IBaseEntity } from '../apiModel'
interface IModuleFunctionModel extends IBaseEntity {
  moduleId: string
  isChecked: boolean
  items: IModuleFunctionModelItem[]
}
interface IModuleFunctionModelItem extends IBaseEntity {
  id: string
  name: string
  isChecked: boolean
}
export type { IModuleFunctionModel }
