import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WBApiService {
  baseUrl = environment.apiUrl;
  headers = new HttpHeaders({
        'Content-Type': 'application/json'
    })


  constructor(private http: HttpClient) { }

  loadOrders(orders: any){
    return this.http.post('https://localhost:5001/api/wbapi/getorders',orders, {headers: this.headers});
  }
}
