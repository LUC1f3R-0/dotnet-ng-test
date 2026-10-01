import { Component, OnInit } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html',
})
export class Login implements OnInit {

  ngOnInit(): void{
    console.log("leaded the loggin");
  }
}
