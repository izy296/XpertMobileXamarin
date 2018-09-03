import { XpertChartComponent } from './../../components/xpert-chart/xpert-chart';
import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { DashboardPage } from './dashboard';

@NgModule({
  declarations: [
    DashboardPage,
    XpertChartComponent
  ],
  imports: [
    IonicPageModule.forChild(DashboardPage),
  ],
})
export class DashboardPageModule {}
