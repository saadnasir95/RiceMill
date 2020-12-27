import { HeadType } from "./enums";

export class Head {
  id: number;
  code: string;
  name: string;
  type: HeadType;
}

export class Head1 extends Head {
  head2: Head2[];
}

export class Head2 extends Head {
  head1Id: number;
  head3: Head3[];
}
export class Head3 extends Head {
  head2Id: number;
  head4: Head4[];
}
export class Head4 extends Head {
  head3Id: number;
  head5: Head5[];
}

export class Head5 extends Head {
  head4Id: number;
}
