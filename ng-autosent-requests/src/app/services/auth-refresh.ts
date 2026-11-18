import { inject, Service } from '@angular/core';
import { finalize, map, Observable, shareReplay } from 'rxjs';
import { AuthService } from './auth';

@Service()
export class AuthRefreshService {
  private readonly authService = inject(AuthService);

  private pendingRefresh$: Observable<void> | null = null;

  // Tracks successful token refresh operations.
  private tokenVersion = 0;

  get version(): number {
    return this.tokenVersion;
  }

  refresh(): Observable<void> {
    // Share an existing refresh request.
    if (this.pendingRefresh$) {
      return this.pendingRefresh$;
    }

    this.pendingRefresh$ = this.authService.refresh().pipe(
      map(response => {
        const { success, message, data } = response;
        if (!response.success) {
          throw new Error(response.message);
        }

        this.tokenVersion++;
      }),

      finalize(() => {
        this.pendingRefresh$ = null;
      }),

      // All subscribers receive the same refresh result.
      shareReplay({
        bufferSize: 1,
        refCount: false
      })
    );

    return this.pendingRefresh$;
  }
}