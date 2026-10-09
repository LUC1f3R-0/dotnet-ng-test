export interface User
{
  Uuid: string,
  name?: string,
  email: string,
  ststus: number,
  isveryfied: boolean,
  role?: Role
}

export enum Role
{
  Removed = 0,
  Active = 1,
  Deactivated = 2
}