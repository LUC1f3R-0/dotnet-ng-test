import { inject } from '@angular/core';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html',
})
export class Login {

  private fb = inject(FormBuilder);

  loginForm = this.fb.nonNullable.group(
    {
      email: ['', [
        Validators.required,
        Validators.email,
      ]],
      passoword: ['', [
        Validators.required,
      ]],
    }
  )
  
  onLogin() {
    if (this.loginForm.invalid)
    {
      this.loginForm.markAllAsTouched();
      return;  
    }
    console.log(this.loginForm.getRawValue());
  }
}
