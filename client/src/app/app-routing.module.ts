import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DetailReportComponent } from './WBApi/detail-report/detail-report.component';
import { FbsReportComponent } from './WBApi/fbs-report/fbs-report.component';
import { SalesReportComponent } from './WBApi/sales-report/sales-report.component';
import { StocksReportComponent } from './WBApi/stocks-report/stocks-report.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'sales-report', component: SalesReportComponent},
  {path: 'stocks-report', component: StocksReportComponent},
  {path: 'detail-report', component: DetailReportComponent},
  {path: 'fbs-report', component: FbsReportComponent},
  {path: '**', component: HomeComponent, pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
