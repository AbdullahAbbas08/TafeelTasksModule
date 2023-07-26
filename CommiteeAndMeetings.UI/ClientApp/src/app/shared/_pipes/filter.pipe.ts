import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(items: any[], searchText: string,value,condtion3,): any[] {
    if(!items){
      return
    }
    if(!searchText || value){
      return items;
    }
    searchText = searchText.toLowerCase().trim();

    return items.filter(trem =>{
        return (
          trem.name?.toLowerCase().includes(searchText) || trem.description?.toLocaleLowerCase().includes(searchText)
        )

        
    })
  }

}