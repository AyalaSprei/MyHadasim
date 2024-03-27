import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ButtonModule } from 'primeng/button';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ChartModule } from 'primeng/chart';
import { HttpClientModule } from '@angular/common/http';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CreateMemberFormComponent } from './create-member-form/create-member-form.component';
import { FormsModule } from '@angular/forms'; // Import FormsModule
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown'; // Import DropdownModule from PrimeNG
import { FileUploadModule } from 'primeng/fileupload';

@NgModule({
  declarations: [
    AppComponent,
    CreateMemberFormComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
   ButtonModule,
   HttpClientModule,
   TableModule,
   ToastModule,
   DialogModule,
   BrowserAnimationsModule,
   FormsModule,
   InputTextModule,
   DropdownModule,
   ChartModule,
   FileUploadModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
