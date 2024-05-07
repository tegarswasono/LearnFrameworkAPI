import type { IBaseEntity } from '../apiModel'
import type { ICategoryModel } from '../category/categoryModel'

interface IProductModel extends IBaseEntity {
  name: string
  description: string
  stok: number
  price: number
  categoryId: string
  category: ICategoryModel
}
interface IProductModelCreateOrUpdate {
  id: string
  name: string
  description: string
  stok: number
  price: number
  categoryId: string
}

export type { IProductModel, IProductModelCreateOrUpdate }
