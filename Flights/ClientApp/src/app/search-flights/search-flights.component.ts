import { Time } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FlightService } from './../api/services/flight.service';
import { FlightRm } from '../api/models';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-search-flights',
  templateUrl: './search-flights.component.html',
  styleUrls: ['./search-flights.component.css']
})
export class SearchFlightsComponent implements OnInit {


  searchResult: FlightRm[] = []

  constructor(private flightService: FlightService,
    private fb: FormBuilder) { }

  searchForm = this.fb.nonNullable.group({
    fromDate: [''],
    toDate: [''],
    from: [''],
    destination: [''],
    numberOfPassengers: [1]
  })

  ngOnInit(): void {
    this.search();
  }

  search() {
    //this.flightService.searchFlight({
    //    fromDate: this.searchForm.controls['fromDate'].value,
    //    toDate: this.searchForm.controls['toDate'].value,
    //    from: this.searchForm.controls['from'].value,
    //    destination: this.searchForm.controls['destination'].value,
    //    numberOfPassengers: this.searchForm.controls['numberOfPassengers'].value,
    //  }).subscribe(response => this.searchResult = response,
    //  this.handleError)

    this.flightService.searchFlight(this.searchForm.value
    ).subscribe(response => this.searchResult = response,
      this.handleError)
  }

  private handleError(err: any) {
    console.log("Response Error. Status: ", err.status)
    console.log("Response Error. Status Text: ", err.statusText)
    console.log(err)
  }

}
