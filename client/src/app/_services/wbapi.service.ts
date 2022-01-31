import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class WBApiService {
  baseUrl = environment.apiUrl;
  headers = new HttpHeaders({
        'Content-Type': 'application/json'
    })


  constructor(private http: HttpClient) { }

  // loadOrders(formParams: any, pageParams: any){
  //   return this.http.post('https://localhost:5001/api/wbapi/getorders', {headers: this.headers});

  //   let params = this.getPaginationHeaders();
  //   return this.getPaginatedResult<string>(this.baseUrl + 'getorders',)
  // }

  loadOrders(formParams: any){
    return this.http.post('https://localhost:5001/api/wbapi/getorders', formParams, {headers: this.headers});
  }

  private getPaginatedResult<T>(url: any, params: any){
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return this.http.get<T>(url, {observe: 'response', params}).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number){
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());

    return params;
  }
}
