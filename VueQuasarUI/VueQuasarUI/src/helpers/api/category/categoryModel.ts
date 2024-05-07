import type { IBaseEntity } from '../apiModel'
interface ICategoryModel extends IBaseEntity {
  name: string
  description: string
}
interface ICategoryModelCreateOrUpdate {
  id: string
  name: string
  description: string
}

export type { ICategoryModel, ICategoryModelCreateOrUpdate }
