import { NgModule } from '@angular/core';
import { MenuSommaireComponent } from './menu-sommaire/menu-sommaire';
import { XpertChartComponent } from './xpert-chart/xpert-chart';
import { EncaissComponent } from './encaiss/encaiss';
import { MenuFilterComponent } from './menu-filter/menu-filter';
@NgModule({
	declarations: [MenuSommaireComponent,
    MenuSommaireComponent,
    MenuSommaireComponent,
    XpertChartComponent,
    EncaissComponent,
    MenuFilterComponent],
	imports: [],
	exports: [MenuSommaireComponent,
    MenuSommaireComponent,
    MenuSommaireComponent,
    XpertChartComponent,
    EncaissComponent,
    MenuFilterComponent]
})
export class ComponentsModule {}
