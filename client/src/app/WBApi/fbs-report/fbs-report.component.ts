import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators  } from '@angular/forms';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { WBApiService } from 'src/app/_services/wbapi.service';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ruLocale } from 'ngx-bootstrap/locale';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { saveAs } from 'file-saver';
defineLocale('ru', ruLocale);

@Component({
  selector: 'app-fbs-report',
  templateUrl: './fbs-report.component.html',
  styleUrls: ['./fbs-report.component.css']
})
export class FbsReportComponent implements OnInit {
  fbsReportForm: FormGroup;
  startDate: Date = new Date();
  orders: any;
  validationErrors: string[] = [];
  user!: User;
  returnedArray?: any[];
  fileName='Orders.xslx';
  
  constructor(private wbapiService: WBApiService, private http: HttpClient,
    private accountService: AccountService, private fb: FormBuilder, localeService: BsLocaleService) {
      accountService.currentUser$.pipe(take(1)).subscribe(user => {
        this.user = user;
      });
      defineLocale('ru', ruLocale);
        localeService.use('ru');
     }

  ngOnInit(): void {
    this.initializeForm();
    
  }

  initializeForm() {
    this.fbsReportForm = this.fb.group({
      date_start: [this.startDate, Validators.required],
      date_end: [''],
      take: [10, Validators.required],
      skip: [0, Validators.required],
      status: [2],
      id: [],
    });
  }
  
  pageChanged(event: PageChangedEvent): void {
    const startItem = (event.page - 1) * event.itemsPerPage;
    const endItem = event.page * event.itemsPerPage;
    this.returnedArray = this.orders.orders.slice(startItem, endItem);
  }

  loadOrders(){
    console.log(this.fbsReportForm.getRawValue());
    this.wbapiService.loadOrders(this.fbsReportForm.getRawValue()).subscribe(response => {
      this.orders = response;
      this.returnedArray = this.orders.orders.slice(0, 10);
    });
  }

  downloadExcel() {
    this.wbapiService.downloadFile(this.orders.orders)
      .subscribe((response: any) => {
        let blob: any = new Blob([response], { type: 'text/json; charset=utf-8' });
        const url = window.URL.createObjectURL(blob);
        //window.open(url);
        saveAs(blob, 'file.xlsx');
      }),
      (error: any) => console.log('Error downloading the file'),
      () => console.info('File downloaded successfully');
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
