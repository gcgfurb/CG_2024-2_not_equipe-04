using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        private const int NumeroPontos = 72;  // Número de pontos para desenhar o círculo
        public double Raio = 0.5;  // Raio do círculo
        public Ponto4D Centro { get; set; }  // Centro do círculo
        public Circulo(Objeto _paiRef, ref char _rotulo, Ponto4D centro, double raio) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.LineLoop;  // Usaremos LineLoop para desenhar o círculo como linhas conectadas
            Raio = raio;
            Centro = centro;
            // Adiciona os pontos do círculo a partir do centro
            AdicionarPontosCirculo(centro);                         
            Atualizar();
        }

        // Função para adicionar os pontos do círculo, distribuídos simetricamente no perímetro
        private void AdicionarPontosCirculo(Ponto4D centro)
        {
            double anguloIncremento = 2 * Math.PI / NumeroPontos;  // Incremento do ângulo para distribuir os pontos
            for (int i = 0; i < NumeroPontos; i++)
            {
                double angulo = i * anguloIncremento;
                double x = centro.X + Raio * Math.Cos(angulo);  // Calcula a coordenada X com base no centro e raio
                double y = centro.Y + Raio * Math.Sin(angulo);  // Calcula a coordenada Y com base no centro e raio
                base.PontosAdicionar(new Ponto4D(x, y));  // Adiciona os pontos calculados
            }
        }

        public void Mover(double deslocamentoX, double deslocamentoY, Circulo circuloMaior)
        {
            // Calcula a nova posição proposta para o centro do círculo
            double novoX = Centro.X + deslocamentoX;
            double novoY = Centro.Y + deslocamentoY;

            // Primeiro, verifica se a nova posição está dentro da BBox interna do círculo maior
            double xmin = circuloMaior.Centro.X - circuloMaior.Raio + this.Raio;
            double xmax = circuloMaior.Centro.X + circuloMaior.Raio - this.Raio;
            double ymin = circuloMaior.Centro.Y - circuloMaior.Raio + this.Raio;
            double ymax = circuloMaior.Centro.Y + circuloMaior.Raio - this.Raio;

            if (novoX < xmin || novoX > xmax || novoY < ymin || novoY > ymax)
            {
                // Não move, fora da BBox interna
                return;
            }

            // Calcula a distância euclidiana ao quadrado entre o novo centro e o centro do círculo maior
            double dx = novoX - circuloMaior.Centro.X;
            double dy = novoY - circuloMaior.Centro.Y;
            double distanciaQuadrada = dx * dx + dy * dy;

            // Distância máxima permitida é (RaioMaior - RaioMenor)
            double distanciaMaxima = circuloMaior.Raio - this.Raio;
            double distanciaMaximaQuadrada = distanciaMaxima * distanciaMaxima;

            if (distanciaQuadrada <= distanciaMaximaQuadrada)
            {
                // Atualiza o centro do círculo
                Centro = new Ponto4D(novoX, novoY);

                // Atualiza os pontos do círculo com base no novo centro
                for (int i = 0; i < PontosListaTamanho; i++)
                {
                    var ponto = PontosId(i);
                    double deslocX = deslocamentoX;
                    double deslocY = deslocamentoY;
                    PontosAlterar(new Ponto4D(ponto.X + deslocX, ponto.Y + deslocY, ponto.Z), i);
                }
                // Redesenha o círculo
                ObjetoAtualizar();
            }
            else
            {
                // Não move, a nova posição está fora dos limites permitidos
                return;
            }
        }


        // Atualiza o objeto no SRU
        private void Atualizar()
        {
            base.ObjetoAtualizar();
        }

#if CG_Debug
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Circulo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
            retorno += base.ImprimeToString();
            return retorno;
        }
#endif
    }
}
