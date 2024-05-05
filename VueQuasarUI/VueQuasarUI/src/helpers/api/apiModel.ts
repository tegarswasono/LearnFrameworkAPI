interface IBaseEntity {
  id: string
  createAt: Date
  updateAt?: Date
}

interface IPagination<T> {
  sortBy: string
  descending: boolean
  page: number
  rowsPerPage: number
  rowsNumber: number

  result: T[]
}

interface IGeneralSuccessResponse {
  message: string
}

export type { IBaseEntity, IPagination, IGeneralSuccessResponse }
