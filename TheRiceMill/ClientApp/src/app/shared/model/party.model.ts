import { Base } from "./base.model";

export class Party extends Base {
  id: number;
  name: string;
  phoneNumber: string;
  address: string;
  createdDate: string;
}
