import { Component, Input } from '@angular/core';

/**
 * Generated class for the XpertChartComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */
@Component({
  selector: 'xpert-chart',
  templateUrl: 'xpert-chart.html'
})
export class XpertChartComponent {

  @Input() title: string;
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  @Input() public barChartLabels: string[] = ["admin", "sysUser"];
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;
  @Input() public barChartData: any[] = [{ data: [0, 0], label: '' },
  { data: [0, 0], label: '' }];
  constructor() {
    console.log('Hello XpertChartComponent Component');
  }

}
