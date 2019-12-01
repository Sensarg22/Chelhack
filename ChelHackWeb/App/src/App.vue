<template>
    <div id="app" class="main">
      <div class="mobile-menu"></div>
      <div id="page-wr">
      <header class="head">
        <div class="head-bot">
          <div class="wrapper">
            <div class="head-bot-cont">
              <div class="main-logo">
                <div class="logo-main"><a href="/"><img src="./assets/img/logo-inner.png" alt=""/></a></div>
              </div>
              <div class="main-menu">
                <nav class="nav">
                  <ul class="nav__list">
                    <li class="nav__item">
                      <router-link class="nav__link" :to="{name: 'home'}" v-bind:class="{'nav__link--active': this.$route.name === 'home'}">Каталог</router-link>
                    </li>
<!--                    <li class="nav__item"><a class="nav__link" href="#">Бренды</a></li>-->
<!--                    <li class="nav__item active"><a class="nav__link" href="#">Книги</a></li>-->
<!--                    <li class="nav__item"><a class="nav__link active" href="#">Магазины</a></li>-->
<!--                    <li class="nav__item"><a class="nav__link" href="#">Сертификаты</a></li>-->
<!--                    <li class="nav__item"><a class="nav__link" href="#">Электроника</a></li>-->
<!--                    <li class="nav__item"><a class="nav__link" href="#">Одежда и обувь</a></li>-->
                    <li class="nav__item">
                      <router-link class="nav__link" :to="{name: 'ShopBasket'}" v-bind:class="{'nav__link--active': this.$route.name === 'ShopBasket'}">Корзина</router-link>
                    </li>
                  </ul>
                </nav>
              </div>
              <div class="burger"><span></span></div>
            </div>
          </div>
        </div>
      </header>

        <div id="content">
          <router-view/>
        </div>

      <footer class="footer">
        <div class="wrapper">
          <div class="footer-cont">
            <div class="footer-el">
              <div class="soc"> <a class="soc__link" href=""><img src="img/ico-vk.svg" alt=""/></a><a class="soc__link" href=""><img src="img/ico-fb.svg" alt=""/></a><a class="soc__link" href=""><img src="img/ico-inst.svg" alt=""/></a><a class="soc__link" href=""><img src="img/ico-tw.svg" alt=""/></a><a class="soc__link" href=""><img src="img/ico-google.svg" alt=""/></a><a class="soc__link" href=""><img src="img/ico-ok.svg" alt=""/></a></div>
              <div class="footer-menu">
                <div class="footer-menu__item"><a class="footer-menu__link" href="#">О компании</a></div>
                <div class="footer-menu__item active"><a class="footer-menu__link" href="#">Новости</a></div>
                <div class="footer-menu__item"><a class="footer-menu__link" href="#">Акции</a></div>
                <div class="footer-menu__item"><a class="footer-menu__link" href="#">Фотогалерея</a></div>
                <div class="footer-menu__item"><a class="footer-menu__link" href="#">Контакты</a></div>
              </div>
            </div>
            <div class="footer-phone">
              <div class="tel-selected"> <a href="tel:+70000000000">+7 (000) 000-00-00</a></div>
            </div>
            <div class="footer-el">
              <div class="footer-el--left">
                <div class="email"><a href="mailto: contact@site.com">contact@site.com</a></div>
              </div>
            </div>
            <div class="footer-el">
              <div class="footer-el--left">
                <div class="copy">© 2018 Корпоративный сайт<br> Все права защищены.</div><a class="politics" href="#">Политика конфиденциальности</a>
              </div>
              <div class="footer-el--right"><a class="dev" href="http://xpage.ru" target="_blank"><span>Сделано в</span>
                <div class="dev__img"><img src="img/ico-dev.svg" alt=""/></div></a></div>
            </div>
          </div>
        </div>
      </footer>
      </div>
    </div>
</template>

<style lang="scss">
  @import 'assets/scss/index';
</style>



