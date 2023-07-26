import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {  SwaggerClient,TransactionBoxDTODataSourceResult } from 'src/app/core/_services/swagger/SwaggerClient.service';



@Injectable({
  providedIn: 'root'
})
export class TransactionService {


  constructor(private swaggerServce: SwaggerClient) { }

  getBoxType(boxType:string,searchText: string,page:number,pageSize:number,committeId:string,isCount:boolean,isEmployee:boolean,filterId:number,filterCase:number):Observable<TransactionBoxDTODataSourceResult>{
     return  this.swaggerServce.apiTransactionsGetboxTypeGet(boxType,searchText, page,pageSize,committeId,isCount,isEmployee,filterId,filterCase)}
}
