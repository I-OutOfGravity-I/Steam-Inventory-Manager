import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table'
import { MatTableModule } from '@angular/material/table'

@Component({
  selector: 'Inventory',
  templateUrl: './Inventory.component.html',
})

export class InventoryComponent {
  public noice: Inventory[] = [];
  dataSource = new MatTableDataSource<Inventory>();
  displayedColumns = [
	'ItemRemovedSymbols',
    'Item',
    'Amount'
  ];
  //@ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<string>(baseUrl + 'inventory').subscribe(result => {
      this.noice = JSON.parse(JSON.stringify(result));
      this.dataSource.data = JSON.parse(JSON.stringify(result));
      //this.dataSource.paginator = this.paginator;
      console.log(this.noice);
    }, error => console.error(error));
  }
}

interface Inventory {
  item: string
  itemRemovedSymbols: string
  amount: number
  icon_url: string
  class_id: string
  rarity_Color: string
  quality: string
  Category: string
  individual_Weapons: Individual_Weapon[]
  tags: Item_Tags[] 
  marketable: number
  market_name: string
  Owner_int: number[]
}

interface Item_Tags {
  tags: string[][]
}

interface Individual_Weapon {
  inspect_link: string
  sticker_link: string[]
  sticker_name: string[]
  Owner: string
  assetid: string
}
