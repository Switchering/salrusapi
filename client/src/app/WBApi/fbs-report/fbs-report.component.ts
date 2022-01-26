import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators  } from '@angular/forms';
import { WBApiService } from 'src/app/_services/wbapi.service';

@Component({
  selector: 'app-fbs-report',
  templateUrl: './fbs-report.component.html',
  styleUrls: ['./fbs-report.component.css']
})
export class FbsReportComponent implements OnInit {
  fbsReportForm!: FormGroup;
  startDate: Date = new Date();
  orders: any;
  validationErrors: string[] = [];
  
  constructor(private wbapiService: WBApiService, private http: HttpClient, 
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.fbsReportForm = this.fb.group({
      date_start: [this.startDate, Validators.required],
      take: ['', Validators.required],
      skip: ['', Validators.required],
    });
  }

  // loadDataFromApi(){
  //   let todayString = this.convertDateToString(this.startDate);
  //   this.fbsUrl = "https://suppliers-api.wildberries.ru/api/v2/orders?date_start=" + todayString + "&status=2&take=1&skip=0"
  //   let authHeaders: HttpHeaders = new HttpHeaders();
  //   authHeaders = authHeaders.append('Accept', 'application/json');
  //   authHeaders = authHeaders.append('Authorization', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NJRCI6IjcyMDk4NzkyLTg1ZDItNGRjZC1iMGNiLWQ0M2FmM2VkZDE1YyJ9.D_pUUY8neP_OobBQEqRtgiqMVTO0Dg2DuQI2UbeXnmA')
  //   this.http.get(this.fbsUrl, {headers: authHeaders}).subscribe(response => {
  //     console.log(response);
  //   })
  // }
 
  loadOrders(){
    this.wbapiService.loadOrders(this.fbsReportForm.getRawValue()).subscribe(response => {
      this.orders = response;
    });
  }

  getHeaders() {
    let headers: string[] = [];
    if(this.orders.orders) {
      this.orders.orders.forEach((value: any) => {
        Object.keys(value).forEach((key) => {
          if(!headers.find((header) => header == key)){
            headers.push(key)
          }
        })
      })
    }
    return headers;
  }

  get _date_start() {
    return this.fbsReportForm.get('date_start')
  }

  get _take() {
    return this.fbsReportForm.get('take')
  }

  get _skip() {
    return this.fbsReportForm.get('skip')
  }

}
