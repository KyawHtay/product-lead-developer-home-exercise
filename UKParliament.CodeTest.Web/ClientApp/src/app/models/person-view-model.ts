import { DepartmentVewModel } from "./department-view-model"

export interface PersonViewModel {
  id: number
  firstName: string
  lastName: string
  dateOfBirth: string
  email: string
  departmentId: number
  department?: DepartmentVewModel
}
