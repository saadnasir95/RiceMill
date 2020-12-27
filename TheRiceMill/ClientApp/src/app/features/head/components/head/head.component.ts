import { Component, OnInit } from '@angular/core';
import { HeadType } from '../../../../shared/model/enums';
import { Head1 } from '../../../../shared/model/head.model';

@Component({
  selector: 'app-head',
  templateUrl: './head.component.html',
  styleUrls: ['./head.component.scss']
})
export class HeadComponent implements OnInit {
  displayedColumns: string[] = ['code', 'name', 'action'];
  panelOpenState = true;
  headType = HeadType;
  head1: Head1[] = [
    {
      id: 1,
      name: 'Assets',
      code: '01-00-00-00-0000',
      type: 0,
      head2: [
        {
          id: 1,
          head1Id: 1,
          name: 'Non Current Assets',
          code: '01-01-00-00-0000',
          type: 0,
          head3: [
            {
              id: 1,
              head2Id: 1,
              name: 'Fixed Assets',
              code: '01-01-01-00-0000',
              type: 0,
              head4: [
                {
                  id: 1,
                  head3Id: 1,
                  name: 'Building',
                  code: '01-01-01-01-0000',
                  type: 0,
                  head5: [
                    {
                      id: 1,
                      head4Id: 1,
                      name: 'Building',
                      code: '01-01-01-01-0001',
                      type: 0,
                    },
                    {
                      id: 2,
                      head4Id: 1,
                      name: 'Building Head Office',
                      code: '01-01-01-01-0002',
                      type: 0,
                    },
                    {
                      id: 3,
                      head4Id: 1,
                      name: 'Building Mill',
                      code: '01-01-01-01-0003',
                      type: 0,
                    }
                  ]
                }

              ]
            }
          ]
        }
      ]
    }
  ]
  constructor() { }

  ngOnInit() {
  }

}
