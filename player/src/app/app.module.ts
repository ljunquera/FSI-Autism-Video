import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from "@angular/forms";
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PatientSummaryTileComponent } from "./patient-summary-tile.component";
import { PatientVideoSummaryComponent } from "./patient-video-summary.component";

@NgModule({
  declarations: [
    AppComponent,
    PatientSummaryTileComponent,
    PatientVideoSummaryComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
