import { NgModule } from '@angular/core';
import { MenuSommaireComponent } from './menu-sommaire/menu-sommaire';
import { XpertChartComponent } from './xpert-chart/xpert-chart';
@NgModule({
	declarations: [MenuSommaireComponent,
    MenuSommaireComponent,
    MenuSommaireComponent,
    XpertChartComponent],
	imports: [],
	exports: [MenuSommaireComponent,
    MenuSommaireComponent,
    MenuSommaireComponent,
    XpertChartComponent]
})
export class ComponentsModule {}
