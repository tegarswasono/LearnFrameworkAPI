import type { IBaseEntity } from '../apiModel'
interface IRoleModel extends IBaseEntity {
  name: string
}
interface IRoleModelCreateOrUpdate {
  id: string
  name: string
}

export type { IRoleModel, IRoleModelCreateOrUpdate }
