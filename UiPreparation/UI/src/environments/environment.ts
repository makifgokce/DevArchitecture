// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  getApiUrl: "https://localhost:5001/api/v1",
  getPageTitle: "DevArchitecture",
  socials:[
    {
      url: 'https://www.instagram.com/tenekekafalar',
      icon: 'instagram',
      title: 'Instagram'
    },
    {
      url: 'https://www.youtube.com/tenekekafalar/',
      icon: 'youtube',
      title: 'Youtube'
    },
    {
      url: 'https://discord.gg/tenekekafalar',
      icon: 'discord',
      title: 'Discord'
    }
  ],
  feign:[
    {
      url: 'https://www.feigngame.com',
      icon: 'hobs',
      title: 'Feign'
    },
    {
      url: 'https://store.steampowered.com/app/1436990/Feign/',
      title: 'Feign Steam'
    },
    {
      url: 'https://discord.gg/rCKCrSK7SY',
      title: 'Feign Discord'
    }
  ],
  hobs:[
    {
      url: 'http://www.hobsgame.com',
      icon: 'hobs',
      title: 'HOBS'
    },
    {
      url: 'https://store.steampowered.com/app/1114860/Hobs/',
      title: 'HOBS Steam'
    }
  ],
  studio: [
    {
      url: 'https://www.youtube.com/c/TenekeKafalarStudios',
      title: 'TK Studios Youtube'
    }
  ]
};
